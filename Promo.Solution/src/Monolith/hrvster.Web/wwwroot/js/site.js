// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('input[type="radio"]').click(function () {
        if ($(this).attr('id') == 'PostNow' && $(this).attr('value') == 'False') {
            $('#scheduleDiv').show();
        }
        else if ($(this).attr('id') == 'PostNow' && $(this).attr('value') == 'True') {
            $('#scheduleDiv').hide();
        }
    });
});