var app = {
    contextMenu: {
        items: [
            {
                element: undefined,
                id: "btn_open",
                type: "button",
                title: "Open",
                isEnabled: false,
                
                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);
                    
                    app.terminal.show();
                }
            },
            {
                element: undefined,
                id: "btn_create_node",
                type: "button",
                title: "Create node",
                isEnabled: true,

                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);
                    console.log(e);
                    
                    var node = app.nodeMap.createNode(e.clientX - 50, e.clientY - 50);
                    
                    var svg = document.getElementsByClassName('surface')[0];
                    svg.appendChild(node.element);
                }
            },
            {
                element: undefined,
                id: "btn_create_joint",
                type: "button",
                title: "Create joint",
                isEnabled: false,

                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);

                    var selectedJoints = app.nodeMap.getSelectedJoints();
                    
                    if (selectedJoints.length === 0) {
                        var id = app.nodeMap.getSelectedNodes()[app.nodeMap.getSelectedNodes().length - 1];
                        var joint = app.nodeMap.createJoint(id);
                        var svg = document.getElementById('joints');
                        svg.appendChild(joint.element);


                        var gArrows = document.getElementById('arrows');
                        gArrows.appendChild(joint.arrowElement);
                        
                        // app.nodeMap.clearBuffer();
                    } else if (selectedJoints.length === 1) {
                        var nodeId = app.nodeMap.getSelectedNodes()[app.nodeMap.getSelectedNodes().length - 1];
                        var node = app.nodeMap.getNodeById(nodeId);
                        var nodePosition = node.getPosition();
                        var joint = app.nodeMap.getSelectedJoints()[0];
                        
                        joint.nodeTo = node.id;
                        joint.setPositionB(nodePosition.x, nodePosition.y);
                        console.log(nodeId);

                        var node_a = app.nodeMap.getNodeById(joint.nodeTo);
                        var node_b = app.nodeMap.getNodeById(joint.nodeFrom);
                        var nodePosA = node_a.getPosition();
                        var nodePosB = node_b.getPosition();
                        var angleRadian = Math.atan2(nodePosA.y - nodePosB.y, nodePosA.x - nodePosB.x);
                        var angle = (angleRadian * 180 / Math.PI);

                        var vec = {
                            x: Math.cos(angleRadian) * -50,
                            y: Math.sin(angleRadian) * -50
                        };

                        var translate = 'translate(' + (nodePosA.x + vec.x) + ',' + (nodePosA.y + vec.y) + ') ';
                        var rotate = 'rotate(' + angle + ')';
                        joint.arrowElement.setAttribute('transform', translate + rotate);
                        
                        node.inputs[joint.id] = joint;
                        
                        app.nodeMap.clearBuffer();
                        app.nodeMap.clearJointsBuffer();
                    }
                    
                    // console.log(nodes);
                }
            },
            {
                element: undefined,
                id: "btn_copy",
                type: "button",
                title: "Copy",
                isEnabled: false,

                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);
                }
            },
            {
                element: undefined,
                id: "btn_joint_delete",
                type: "button",
                title: "Delete",
                isEnabled: false,

                click: function (e) {
                    console.log("joint" + this.title);
                    console.log(this.element);
                    
                    var jointId = app.nodeMap.getSelectedJoints()[0];
                    
                    app.nodeMap.removeJoint(jointId);
                    app.nodeMap.clearJointsBuffer();
                }
            },
            {
                element: undefined,
                id: "btn_paste",
                type: "button",
                title: "Paste",
                isEnabled: false,

                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);
                }
            },
            {
                element: undefined,
                id: "btn_cut",
                type: "button",
                title: "Cut",
                isEnabled: false,

                click: function (e) {
                    console.log(this.title);
                    console.log(this.element);
                }
            },
            {
                type: "line"
            },
            {
                element: undefined,
                id: "btn_clear",
                type: "button",
                title: "Clear",
                isEnabled: true,

                click: function (e) {
                    // console.log(this.title);
                    // console.log(this.element);

                    app.nodeMap.clearBuffer();
                    app.nodeMap.clearNodes();
                }
            },
            {
                element: undefined,
                id: "btn_delete",
                type: "button",
                title: "Delete",
                isEnabled: false,
                
                click: function (e) {
                    // console.log(this.title);
                    // console.log(this.element);
                    // console.log(e);
                    
                    var id = app.nodeMap.getSelectedNodes()[app.nodeMap.getSelectedNodes().length - 1];
                    
                    app.nodeMap.removeNode(id);
                    // app.nodeMap.removeNode()
                }
            }
        ],
        getItemById: function(id) {
            for (var i = 0; i < this.items.length; i++) {
                if (id === this.items[i].id) {
                    return this.items[i];
                }
            }
        },
        setEnableItem: function (id, isEnabled) {
            var item = this.getItemById(id);
            item.isEnabled = isEnabled;
            
            if (isEnabled) {
                item.element.setAttribute('class', 'context_item');
            }
            else {
                item.element.setAttribute('class', 'context_item disabled');
            }
        }
    },
    nodeMap: {
        nodes: {
            // element
            // id
            // title
            // x
            // y
            // inputs
            // outputs
        },
        joints: {
            // element
            // id,
            // nodeFrom,
            // nodeTo
        },
        selectedNodes: [], // list buffer nodes ids
        selectedJoints: [], // list buffer joints ids
        
        createJoint: function(fromNodeId) {
            var id = this.generateGuid();
            var node = this.getNodeById(fromNodeId);
            var nodePosition = node.getPosition();
            // var nodePosition = this.getNodePosition(node);
            var g = document.createElementNS('http://www.w3.org/2000/svg', 'line');
            g.setAttribute('id', 'joint');
            g.setAttribute('x1', nodePosition.x);
            g.setAttribute('y1', nodePosition.y);

            g.setAttribute('x2', nodePosition.x);
            g.setAttribute('y2', nodePosition.y);
            
            g.setAttribute('style', " stroke:rgb(255,255,255);stroke-width:3");
            // g.setAttribute('transform', 'translate(' + x + ',' + y + ')');
            g.setAttribute('key', id);


            var gA = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            gA.setAttribute('transform', 'translate(' + nodePosition.x + ',' + nodePosition.y + ') rotate(0)')
            var arrow = document.createElementNS('http://www.w3.org/2000/svg', 'polyline');
            arrow.setAttribute("transform", 'translate(-45, -38)')
            arrow.setAttribute('fill', 'none');
            arrow.setAttribute('stroke', '#fff');
            arrow.setAttribute('stroke-width', '3');
            arrow.setAttribute('stroke-linecap', 'round');
            arrow.setAttribute('points', '0,0 45,38 0,75');
            
            gA.appendChild(arrow);
            // "            <polyline =\"none\" stroke=\"#FFFFFF\" stroke-width=\"1\" stroke-linecap=\"round\" stroke-linejoin=\"round\" points=\"\n" +
            //     "    0,0 45,38 0,75 \"/>";
            g.onclick = function (ev) { 
                console.log(ev);
            };
            
            var joint = {
                element: g,
                id: id,
                nodeFrom: fromNodeId,
                nodeTo: null,
                arrowElement: gA,
                
                setPositionA: function (x, y) {
                    this.element.setAttribute('x1', x);
                    this.element.setAttribute('y1', y);
                },
                setPositionB: function (x, y) {
                    this.element.setAttribute('x2', x);
                    this.element.setAttribute('y2', y);
                }
            };
            

            this.joints[joint.id] = joint;
            
            this.selectedJoints.push(joint);
            
            node.outputs[joint.id] = joint;

            return joint;
        },
        getSelectedJoints: function() {
            return this.selectedJoints;
        },
        getJointById: function(id) {
            return this.joints[id];
        },
        
        generateGuid: function() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        },

        selectJoint: function(id) {
            var index = -1;
            for (var i = 0; i < this.selectedJoints.length; i++) {
                if (this.selectedJoints[i] === id) {
                    index = i;
                    break;
                }
            }

            if (index !== -1) {
                this.selectedJoints.splice(index, 1);
            }

            this.selectedJoints.push(id);
        },
        selectNode: function(id) {
            var index = -1;
            for (var i = 0; i < this.selectedNodes.length; i++) {
                if (this.selectedNodes[i] === id) {
                    index = i;
                    break;
                }
            }
            
            if (index !== -1) {
                this.removeFromBufferByIndex(index);
            }

            this.selectedNodes.push(id);
        },
        getSelectedNodes: function() {
            return this.selectedNodes;
        },
        getNodeById: function(id) {
            return this.nodes[id];
        },
        removeJoint: function(id) {
            var joint = this.getJointById(id);
            var nodeA = this.getNodeById(joint.nodeFrom);
            var nodeB = this.getNodeById(joint.nodeTo);
            
            for (var output in nodeA.outputs) {
                delete nodeA.outputs[output];
            }

            for (var input in nodeB.inputs) {
                delete nodeB.inputs[input];
            }
            
            document.getElementById("joints").removeChild(joint.element);
            document.getElementById("arrows").removeChild(joint.arrowElement);
            
            delete this.joints[id];
        },
        createNode: function (x, y) {
            var id = this.generateGuid();
            var g = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            g.setAttribute('id', 'node');
            g.setAttribute('transform', 'translate(' + x + ',' + y + ')');
            g.setAttribute('key', id)
            
            g.innerHTML = "<circle cx=\"0\" cy=\"0\" r=\"50\" class=\"node\" />\n" +
                "            <text x=\"0\" y=\"0\" class=\"text-align\">\n" +
                "                <tspan x=\"0\" dy=\"-1em\">\n" +
                "                    I\n" +
                "                </tspan>\n" +
                "                <tspan x=\"0\" dy=\"1em\">\n" +
                "                    Not\n" +
                "                </tspan>\n" +
                "                <tspan x=\"0\" dy=\"1em\">\n" +
                "                    Understand\n" +
                "                </tspan>\n" +
                "            </text>";
            
            var node = {
                element: g,
                id: id,
                title: "",
                x: x,
                y: y,
                inputs: {},
                outputs: {},
                getPosition: function () {
                    var transform = this.element.getAttribute('transform');
                    var translateArray = transform
                        .replace('translate', '')
                        .replace('(', '')
                        .replace(')', '')
                        .split(',');

                    var translate = {
                        x: parseFloat(translateArray[0]),
                        y: parseFloat(translateArray[1])
                    };
                    
                    return translate;
                }
            };
            
            this.nodes[node.id] = node;
            
            return node;
        },
        removeNode: function (id) {
            // this.removeFromBuffer(id);
            this.clearBuffer();
            
            var svg = document.getElementsByClassName('surface')[0];
            var node = this.getNodeById(id);
            
            svg.removeChild(node.element);
            
            delete this.nodes[node.id];
        },
        removeFromBufferById: function (id) {
            var index = -1;
            
            for (var i = 0; i < this.selectedNodes.length; i++) {
                if (this.selectedNodes[i] === id) {
                    index = i;
                    break;
                }
            }
            
            this.removeFromBufferByIndex(index);
        },
        removeFromBufferByIndex: function (index) {
            this.selectedNodes.splice(index, 1);
        },
        clearBuffer: function () {
            this.selectedNodes.splice(0, this.selectedNodes.length);
        },
        clearJointsBuffer: function() {
            this.selectedJoints.splice(0, this.selectedJoints.length);
        },
        clearNodes: function () {
            for (var nodeId in this.nodes) {
                this.removeNode(nodeId)
            }
        },
    },
    projects: {
        items: [
            {
                id: 'djwakdjla',
                type: 'folder',
                title: 'test',
                files: []
            },
            {
                id: 'djwakdjla',
                type: 'file',
                title: 'test'
            }
        ],
    }
};