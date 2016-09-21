// tell JSLint to suppress errors relating to objects declared elsewhere. Not strictly essential if you don't use JSLint
/*global $, ko, AllPostsViewModel */

function init() {

    ShowProcessing('Loading posts ...');

    var allPostsVM = new AllPostsViewModel();
    $.when(allPostsVM.LoadPosts()).done(function () {
        setTimeout(function () { ko.applyBindings(allPostsVM, document.getElementById("all-posts-section")); }, 500);
        setTimeout(function () { HideProcessing(); }, 1000);
    });

}

RegisterOnload(init);
