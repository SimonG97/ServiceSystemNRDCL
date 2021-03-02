// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var page = sessionStorage.getItem('page');
    var navbar = document.querySelectorAll('li');
    navbar.forEach((item, index) => {
        if (item.children[0].href == page) {
            $(item).addClass('active').siblings().removeClass('active');
        }
    });

})

$(document).on('click', 'ul li', function () {
    var x = this.children[0].href;
    sessionStorage.setItem('page', x);
});