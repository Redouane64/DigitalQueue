// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $('.course-teacher').select2({
        ajax: {
            method: 'POST',
            url: '/api/teachers/search',
            delay: 300,
        },
        placeholder: 'Search for a teachers',
        minimumInputLength: 2,
    });

    $('.user-roles').select2({
        ajax: {
            method: 'POST',
            url: '/api/roles/search',
            delay: 300
        },
        placeholder: 'Search for roles',
        minimumInputLength: 2,
    });
    
    $('.send-code').click(function () {
        $.ajax('/api/accounts/request-password-reset', { 
            method: 'POST',
            success: function (data,status,xhr) { 
                $('a.btn-outline-dark')
                    .addClass('btn-outline-success')
                    .removeClass('btn-outline-dark')
                    .addClass('disabled');
                
                $('a.send-code').text('Code Sent!');
            },
            error: function (jqXhr, textStatus, errorMessage) {
                $('a.btn-outline-dark')
                    .addClass('btn-outline-danger')
                    .removeClass('btn-outline-dark');
                
                $('a.send-code').text('Try Again!');
            }
        });
    })
});