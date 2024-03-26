// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#immediateRadioDiv').click(function () {
        $('[name="PostNow"]').removeAttr('checked');
        var value = "true";
        $("input[name=PostNow][value=" + value + "]").attr('checked', 'checked');
    });

    $('#showOnScreenRadioDiv').click(function () {
        $('[name="PostNow"]').removeAttr('checked');
        var value = "false";
        $("input[name=PostNow][value=" + value + "]").attr('checked', 'checked');
    })

    $("#sweepEntryTable").DataTable({
        destroy: true,
        searching: true,
    });

    $(document).on('click', '.prize-image-box', function () {
        $('.prize-image-box').removeClass('selected');
        $(this).addClass('selected');

        var imageUid = $(this).data('uid');

        $('#ImageUid').attr('value', imageUid);
    });
});