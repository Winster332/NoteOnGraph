// $(document.getElementsByClassName("workspace")[0]).on("contextmenu", function(event) {

$(document).on("contextmenu", function(event) {
    var element = event.target;
    var type = element.getAttribute('class');

    console.log(type);
    if (type === 'node') {
        app.contextMenu.setEnableItem('btn_open', true);
        app.contextMenu.setEnableItem('btn_copy', true);
        app.contextMenu.setEnableItem('btn_cut', true);
        app.contextMenu.setEnableItem('btn_delete', true);
        app.contextMenu.setEnableItem('btn_create_joint', true);
        
        app.contextMenu.setEnableItem('btn_create_node', false);
        app.contextMenu.setEnableItem('btn_paste', false);
        app.contextMenu.setEnableItem('btn_clear', false);
        app.contextMenu.setEnableItem('btn_joint_delete', false);
    } else if (type === 'surface') {
        app.contextMenu.setEnableItem('btn_create_node', true);
        app.contextMenu.setEnableItem('btn_paste', true);
        app.contextMenu.setEnableItem('btn_clear', true);
        
        app.contextMenu.setEnableItem('btn_open', false);
        app.contextMenu.setEnableItem('btn_copy', false);
        app.contextMenu.setEnableItem('btn_cut', false);
        app.contextMenu.setEnableItem('btn_delete', false);
        app.contextMenu.setEnableItem('btn_create_joint', false);
        app.contextMenu.setEnableItem('btn_joint_delete', false);
    } else if (type === null) {
        var elementId = element.id;
        
        if (elementId === 'joint') {
            app.contextMenu.setEnableItem('btn_clear', true);
            app.contextMenu.setEnableItem('btn_create_node', true);
            app.contextMenu.setEnableItem('btn_joint_delete', true);
            
            app.contextMenu.setEnableItem('btn_open', false);
            app.contextMenu.setEnableItem('btn_copy', false);
            app.contextMenu.setEnableItem('btn_cut', false);
            app.contextMenu.setEnableItem('btn_delete', false);
            app.contextMenu.setEnableItem('btn_create_joint', false);
            app.contextMenu.setEnableItem('btn_paste', false);
            
            var jointId = element.getAttribute('key');
            app.nodeMap.selectJoint(jointId);
        }
    }
    
    event.preventDefault();
    $(".context")
        .show()
        .css({
            top: event.pageY - 50,
            left: event.pageX - 60
        });
});
$(document).click(function(e) {
    if ($(".context").is(":hover") == false) {
        $(".context").fadeOut("fast");
    };
});


// $(document.getElementById("left-panel")).on("contextmenu", function(event) {
//     event.preventDefault();
//     $(".context")
//         .show()
//         .css({
//             top: event.pageY - 50,
//             left: event.pageX - 60
//         });
// });
// $(document.getElementById("left-panel")).click(function(e) {
//     if ($(".context").is(":hover") == false) {
//         $(".context").fadeOut("fast");
//     };
// });


(function () {

    var context = document.getElementById('context-menu');
    var contextModel = app.contextMenu;

    for (var i = 0; i < contextModel.items.length; i++) {
        var currentItem = contextModel.items[i];
        
        if (currentItem.type === "button") {
            var item = document.createElement('div');
            if (currentItem.isEnabled) {
                item.setAttribute('class', 'context_item');
            } else {
                item.setAttribute('class', 'context_item disabled');
            }

            var innerItem = document.createElement('div');
            
            
            innerItem.setAttribute('class', 'inner_item');
            innerItem.setAttribute('id', currentItem.id);
            innerItem.innerText = currentItem.title;
            item.appendChild(innerItem);

            item.onclick = function (ev) {
                $(".context").fadeOut("fast");
                
                var element = ev.composedPath()[0];
                var localContextItem = contextModel.getItemById(element.id);
                
                localContextItem.click(ev);
            };
            
            context.appendChild(item);
            currentItem.element = item;
        } else if (currentItem.type === "line") {
            var contextLine = document.createElement('div');
            contextLine.setAttribute('class', 'context_hr');
            
            context.appendChild(contextLine);
        }
    }
})();
