
function Dialogs() {
    var _this = this;
    var dialogType = function(){
        return {
            'DeleteDialog':0,
            'DownloadDialog':1,
            'ErrorDialog':2};
    }();
    this.DialogType = dialogType;
    function typicalDialog() {
        _this = this;
        var dialog,
            agreeCallback,
            dialogMsg;
        this.setDialog = function(dialogParam) {
            dialog = dialogParam;
            _this.agreeCallback = null;
        };
        this.setAgreeCallback = function(callback) {
            agreeCallback = callback;
        };
        this.setDialogMessage = function(msg) {
            dialogMsg = msg;
        };
        this.showDialog = function() {
            if (agreeCallback != null) {
                var buttons = dialog.dialog("option", "buttons");
                buttons.Ok = agreeCallback;
                dialog.dialog("option", "buttons", buttons);
            }
            if (dialogMsg != null) {
                //text part or fully formed message part
                $(dialog).find(".dialogText").text(dialogMsg);
            }
            dialog.dialog("open");
        };
        this.closeDialog = function() {
            dialog.dialog("close");
        };

    }
    var delDialog = new typicalDialog(),
        downDialog = new typicalDialog(),
        errDialog = new typicalDialog();
    
    function deleteDialog(agreeCallback){
        return $("#dialog-confirm").dialog({
            resizable: false,
            height: 200,
            width: 400,
            modal: true,
            autoOpen: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    };
    function downloadDialog() {
        return $("#dialog-download").dialog({
            resizable: false,
            height: 200,
            width: 400,
            modal: true,
            autoOpen: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    };
    function errorDialog() {
        return $("#dialog-error").dialog({
            resizable: false,
            height: 200,
            modal: true,
            autoOpen: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    };
    this.init = function () {
        delDialog.setDialog(deleteDialog());
        
        downDialog.setDialog(downloadDialog());
        
        errDialog.setDialog(errorDialog());
        
    };
    this.getDialog = function(dType) {
        switch (dType) {
            case dialogType.DeleteDialog:
                return delDialog;
            case dialogType.DownloadDialog:
                return downDialog;
            default:
                return errDialog;
        }
    };
}

function DeleteItem(dialog, ajaxCaller, errorDialog) {
    var deleteUrl = "/Home/Delete";
    this.init = function() {
        $(".removeFileIcon").click(function() {
            var id = $(this).data("id");
            var fullItemName = $.trim($(this).parent().find(".fileName").attr("title"));
            dialog.setDialogMessage(fullItemName);
            dialog.setAgreeCallback(function () {
                dialog.closeDialog();
                confirmDelete(id);
            });
            dialog.showDialog();
        });
    };
    function confirmDelete(idParam) {
        ajaxCaller.get(deleteUrl, { id: idParam },
            function (dataFromServer) {
                if (dataFromServer.success) {
                    window.location.reload();
                } else {
                    errorDialog.setDialogMessage(dataFromServer.error);
                    errorDialog.showDialog();
                }
            });
    }
}

function DownloadItem(dialog, ajaxCaller) {
    var downloadUrl = "Home/Download";
    this.init = function () {
        $(".downloadFileIcon").click(function () {
            var id = $(this).data("id");
            var fullItemName = $.trim($(this).parent().find(".fileName").attr("title"));
            dialog.setDialogMessage(fullItemName);
            dialog.setAgreeCallback(function () {
                dialog.closeDialog();
                confirmDownload(id);
            });
            dialog.showDialog();
        });
    };
    function confirmDownload(idParam) {
        downloadByUrl(downloadUrl + "?id=" + idParam);
    }
    var downloadByUrl = function (url) {
        var hiddenIFrameId = 'hiddenDownloader',
            iframe = document.getElementById(hiddenIFrameId);
        if (iframe === null) {
            iframe = document.createElement('iframe');
            iframe.id = hiddenIFrameId;
            iframe.style.display = 'none';
            document.body.appendChild(iframe);
        }
        iframe.src = url;
    };

}

function AjaxCaller(onErrorCallback) {
    this.get = function (url, data, onSucessCallback ) {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function(fromServerData) {
                onSucessCallback(fromServerData);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (onErrorCallback != null) {
                    onErrorCallback();
                }
            }
    });
    };
}

function AjaxFileUploader(errorDialog) {
    this.init = function() {
        $('#fileupload').fileupload({
            dataType: 'json',
            done: function (e, data) {
                if (!data.result.success) {
                    errorDialog.setDialogMessage(data.result.error);
                    errorDialog.showDialog();
                } else {
                    //return to the start page
                    window.location.replace("/");
                }


            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormsg = ""
                if (jqXHR.status == 404) {//file size limit from Web Server
                    errormsg = "File size limit. Please use max size 10Mb";
                } else {
                    errormsg = "Incorrect response from server. PLease try later.";
                }
                errorDialog.setDialogMessage(errormsg);
                errorDialog.showDialog();
            }
        });
    };
}

var dialogs,
    ajaxCaller;


$(document).ready(function () {
   dialogs = new Dialogs();
   dialogs.init();
   ajaxCaller = new AjaxCaller(function() {
        var errorDialog = dialogs.getDialog(dialogs.DialogType.ErrorDialog);
        errorDialog.setDialogMessage("Problem with connection to server, please try later.");
        errorDialog.showDialog();
   });
    new DeleteItem(dialogs.getDialog(dialogs.DialogType.DeleteDialog), ajaxCaller,
        dialogs.getDialog(dialogs.DialogType.ErrorDialog)).init();
   new DownloadItem(dialogs.getDialog(dialogs.DialogType.DownloadDialog), ajaxCaller).init();
   new AjaxFileUploader(dialogs.getDialog(dialogs.DialogType.ErrorDialog)).init();
});