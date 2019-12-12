$(".openbtn").on("click", function() {
    $(".ui.sidebar").toggleClass("very thin icon");
    $(".asd").toggleClass("marginlefting");
    $(".sidebar z").toggleClass("displaynone");
    $(".ui.accordion").toggleClass("displaynone");
    $(".ui.dropdown.item").toggleClass("displayblock");

    $(".logo").find('img').toggle();

})
$(".ui.dropdown").dropdown({
    allowCategorySelection: true,
    transition: "fade up",
    context: 'sidebar',
    on: "hover"
});

$('.ui.accordion').accordion({
    selector: {

    }
});


(function() {
    var mouseDown = {
        isDown: false,
        x: 0,
        y: 0,
        nodeElement: undefined,
        selected: {
        }
    };


    document.getElementsByClassName('surface')[0].addEventListener('mousedown', function(e) {
        var type = e.composedPath()[0].className.baseVal;
        
        if (type === 'node') {
            var nodeGroup = e.composedPath()[1];//[0].className.baseVal;

            if (e.button === 0) {
                mouseDown.x = e.clientX;
                mouseDown.y = e.clientY - 60;
                mouseDown.nodeElement = nodeGroup;
                mouseDown.isDown = true;
            }
            else if (e.button === 2) {
                var nodeId = nodeGroup.getAttribute('key');
                app.nodeMap.selectNode(nodeId);
                mouseDown.selected[nodeId] = nodeId;
                // var element = e.composedPath()[0];
                // var type = element.getAttribute('class');
                //
                // if (type === 'node') {
                //     app.contextMenu.setEnableItem('btn_open', true);
                //     app.contextMenu.setEnableItem('btn_create_node', false);
                // }
            }
        }
    });
    document.getElementsByClassName('surface')[0].addEventListener('mouseup', function(e) {
        mouseDown.isDown = false;
        
        for (var nodeId in mouseDown.selected) {
            app.nodeMap.removeFromBufferById(nodeId);
            
            delete mouseDown.selected[nodeId];
        }
        
        app.nodeMap.clearJointsBuffer();
        
        if (app.terminal.isShow) {
            app.terminal.hide();
        }
    });
    
    document.getElementsByClassName('surface')[0].addEventListener('mousemove', function(e) {
        var x = e.clientX - 50;
        var y = e.clientY - 60;
        
        if (mouseDown.isDown) {

            // var translateArray = e.currentTarget.getAttribute("transform")
            //     .replace('translate', '')
            //     .replace('(', '')
            //     .replace(')', '')
            //     .split(',');
            //
            // var translate = {
            //     x: translateArray[0],
            //     y: translateArray[1]
            // };

            mouseDown.nodeElement.setAttribute('transform', 'translate(' + x + ',' + y + ')');
            var node = app.nodeMap.getNodeById(mouseDown.nodeElement.getAttribute('key'));
            
            for (var inputId in node.inputs) {
                var joint = app.nodeMap.getJointById(inputId);
                joint.setPositionB(x, y);

                var node_a = app.nodeMap.getNodeById(joint.nodeFrom);
                var nodePos = node_a.getPosition();
                var angleRadian = Math.atan2(nodePos.y - y, nodePos.x - x);
                var angle = (angleRadian * 180 / Math.PI) + 180;
                
                var vec = {
                    x: Math.cos(angleRadian) * 50,
                    y: Math.sin(angleRadian) * 50
                };

                var translate = 'translate(' + (x + vec.x) + ',' + (y + vec.y) + ') ';
                var rotate = 'rotate(' + angle + ')';
                joint.arrowElement.setAttribute('transform', translate + rotate)
            }

            for (var outputId in node.outputs) {
                var joint = app.nodeMap.getJointById(outputId);
                joint.setPositionA(x, y);

                var node_a = app.nodeMap.getNodeById(joint.nodeTo);
                var nodePos = node_a.getPosition();
                var angleRadian = Math.atan2(nodePos.y - y, nodePos.x - x);
                var angle = (angleRadian * 180 / Math.PI);

                var vec = {
                    x: Math.cos(angleRadian) * -50,
                    y: Math.sin(angleRadian) * -50
                };

                var translate = 'translate(' + (nodePos.x + vec.x) + ',' + (nodePos.y + vec.y) + ') ';
                var rotate = 'rotate(' + angle + ')';
                joint.arrowElement.setAttribute('transform', translate + rotate)
            }
        }
        
        if (app.nodeMap.selectedJoints.length !== 0) {
            var joint = app.nodeMap.selectedJoints[0];
            
            console.log(joint);
            joint.setPositionB(x, y);

            var node = app.nodeMap.getNodeById(joint.nodeFrom);
            var nodePos = node.getPosition();

            var angle = Math.atan2(nodePos.y - y, nodePos.x - x);
            var translate = 'translate(' + x + ',' + y + ') ';
            var rotate = 'rotate(' + ((angle * 180 / Math.PI) + 180) + ')';
            joint.arrowElement.setAttribute('transform', translate + rotate)
        }
    });

    // document.getElementById('node').addEventListener('mousedown', function(e) {
    //     // e.currentTarget.setAttribute('fill', '#ff00cc');
    // });
    //
    // document.getElementById('node').addEventListener('mouseup', function(e) {
    //     // e.currentTarget.setAttribute('fill', '#ff00cc');
    // });
    //
    // document.getElementById('node').addEventListener('mousemove', function(e) {
    //     // e.currentTarget.setAttribute('fill', '#ff00cc');
    // });
}());

