app.terminal = {
    isShow: false,
    
    show: function (node) {
        this.isShow = true;
        
        $(".terminal-window")
            .show()
            .css({
                // top: event.pageY - 50,
                // left: event.pageX - 60
            });
    },
    hide: function () {
        this.isShow = false;
        
        $(".terminal-window").fadeOut("fast");
        
        document.getElementsByClassName('terminal-editor')[0].value = "";
    },
    buttons: {
        close: function () {
            app.terminal.hide();
        },
        hide: function () {
        },
        maximize: function () {
            
        }
    }
};

