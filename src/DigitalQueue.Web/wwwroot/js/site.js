// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $('.course-teacher').select2({
        ajax: {
            method: 'POST',
            url: '/api/teachers/search',
            delay: 300,
            contentType: 'application/json'
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
});