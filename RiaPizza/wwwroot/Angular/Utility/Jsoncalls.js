function JsonCallParam(Controller, Action, Parameters) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/' + Controller + '/' + Action,
        context: document.body,
        data: Parameters,
        success: function (json) {
            list = null;
            list = json;
        }
        ,
        error: function (xhr) {
            list = null;
        }
    });
}
function JsonCall(Controller, Action) {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/' + Controller + '/' + Action,
        context: document.body,
        success: function (json) {
            list = null; list = json;
        },
        error: function (xhr) {
            list = null;
            //debugger;
        }
    });
}