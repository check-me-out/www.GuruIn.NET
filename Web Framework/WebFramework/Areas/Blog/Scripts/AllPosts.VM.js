// tell JSLint to suppress errors relating to objects declared elsewhere. Not strictly essential if you don't use JSLint
/*global $, window, ko, Enumerable, allPostsApi, summaries*/

/// <reference path="~/Areas/Blog/Scripts/AllPosts.Api.js"/>

var AllPostsViewModel = function () {
    var self = this;

    // observable arrays are update binding elements upon array changes
    self.Posts = ko.observableArray([]);
    self.TotalNumberOfPosts = ko.observable(-1);
    self.NumberOfPostsInCurrentPage = ko.observable(-1);

    self.UrlSlug = ko.observable('');
    self.Category = ko.observable('');
    self.Tag = ko.observable('');
    self.Archive = ko.observable('');
    self.SearchTerm = ko.observable('');
    self.CurrPage = ko.observable('');
    self.PrevPageUrl = ko.observable('');
    self.NextPageUrl = ko.observable('');

    self.LoadPosts = function () {

        ShowProcessing('Loading posts ...');

        var cat = getQsValue('category');
        cat = !cat ? '' : cat;
        var tag = getQsValue('tag');
        tag = !tag ? '' : tag;
        var archive = getQsValue('archive');
        archive = !archive ? '' : archive;
        var search = getQsValue('searchTerm');
        search = !search ? '' : search;

        var result = window.allPostsApi.client.getAllPosts('', cat, tag, 0, archive, search).done(function (data) {

            self.ProcessResult(data);

        }).error(function (d) {
            alertify('An error occured while retrieving posts. Please try again alater.', AlertSeverity.ERROR);
        });
    };

    self.UpdatePosts = function (url) {
        ShowProcessing('Showing relevant posts ...');

        var result = window.allPostsApi.client.invoke(url).done(function (data) {

            self.ProcessResult(data);

            HideProcessing();
        }).error(function (d) {
            alertify('An error occured while retrieving posts. Please try again alater.', AlertSeverity.ERROR);
            HideProcessing();
        });
    };

    self.ProcessResult = function (data) {
        data.UrlSlug = !data.UrlSlug ? '' : data.UrlSlug;
        data.Category = !data.Category ? '' : data.Category;
        data.Tag = !data.Tag ? '' : data.Tag;
        data.Archive = !data.Archive ? '' : data.Archive;
        data.SearchTerm = !data.SearchTerm ? '' : data.SearchTerm;
        data.CurrPage = !data.CurrPage ? '' : data.CurrPage;

        if (data.Category) {
            $("#blog-menu-categories ul li").each(function (e) {
                var cat = $(this).find('a').first().html();
                if (cat && cat.toLowerCase() == data.Category.toLowerCase()) {
                    $(this).find('a').first().addClass('highlight');
                    $("#blog-menu-categories button").html('<span class="button-text">' + cat + '</span> <span class="caret-holder"> <span class="caret"></span> </span>');
                }
                else {
                    $(this).find('a').first().removeClass('highlight');
                }
            });
        }
        else {
            $("#blog-menu-categories ul li").each(function (e) {
                var cat = $(this).find('a').first().html();
                if (cat == 'All Categories') {
                    $(this).find('a').first().addClass('highlight');
                    $("#blog-menu-categories button").html('<span class="button-text">' + cat + '</span> <span class="caret-holder"> <span class="caret"></span> </span>');
                }
                else {
                    $(this).find('a').first().removeClass('highlight');
                }
            });
        }

        $("#blog-menu-tags .tag-link-contents a").each(function (e) {
            var tag = $(this).html();
            if (tag && tag.toLowerCase() == data.Tag.toLowerCase()) {
                $(this).addClass('selected');
            }
            else {
                $(this).removeClass('selected');
            }
        });

        if (data.SearchTerm) {
            $('input[name=\'searchTerm\']').val(data.SearchTerm);
            var times = '<div class="search-button input-group-addon close-search"><i class="fa fa-times"></i></div>';
            $(times).insertAfter('input[name=\'searchTerm\']');
            $(".close-search").click(function (e) {
                $('input[name=\'searchTerm\']').val('');
                $('form').submit();
            });
        }

        var postsArray = Enumerable.From(data.AllPosts)
            .Select(
                function (post) { // Element selector

                    var tagsArray = Enumerable.From(post.Tags)
                                .Select(function (tag) {
                                    tag = {
                                        Parent: self,
                                        Id: tag.Id,
                                        Name: tag.Name,
                                        UrlSlug: tag.UrlSlug,
                                        Description: tag.Description,
                                        Class: tag.Class,
                                        TagHref: '/Blog/AllPosts/' + tag.UrlSlug + '?tag=' + tag.Name + '&category=' + data.Category,
                                        SelectTag: function () {
                                            var href = this.TagHref.replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
                                            self.UpdatePosts(href);
                                        },
                                        SelectedTagHref: '/Blog/AllPosts?category=' + data.Category,
                                        DeselectTag: function () {
                                            var href = this.SelectedTagHref.replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
                                            self.UpdatePosts(href);
                                        }
                                    };

                                    return tag;
                                })
                                .OrderBy(function (tag) { return tag.Name })
                                .ToArray();

                    var strPostedOn = post.PostedOn.toString().replace('T', ' ');
                    var formattedPostedOn = $.datepicker.formatDate('DD, MM dd, yy', $.datepicker.parseDate("yy-mm-dd", strPostedOn));

                    var formattedModifiedOn = '';
                    if (post.ModifiedOn != null) {
                        var strModifiedOn = post.ModifiedOn.toString().replace('T', ' ');
                        var formattedModifiedOn = $.datepicker.formatDate('MM dd, yy', $.datepicker.parseDate("yy-mm-dd", strModifiedOn));
                    }

                    post = {
                        Parent: self,
                        Id: post.Id,
                        Title: post.Title,
                        ShortDescription: post.ShortDescription,
                        ShorterDescription: post.ShortDescription.substring(0, 150),
                        UrlSlug: post.UrlSlug,
                        Published: post.Published,
                        PostedOn: formattedPostedOn,
                        ModifiedOn: formattedModifiedOn,
                        Category: {
                            Id: post.Category.Id,
                            Name: post.Category.Name,
                            UrlSlug: post.Category.UrlSlug,
                            Description: post.Category.Description,
                        },
                        Tags: ko.observableArray(tagsArray),
                        PostHref: '/Blog/Post/' + post.Id + '/' + post.UrlSlug + '?category=' + data.Category + '&tag=' + data.Tag + '&cur=' + data.CurrPage + '&searchTerm=' + data.SearchTerm,
                        CategoryHref: '/Blog/AllPosts/' + post.Category.UrlSlug + '?category=' + post.Category.Name + '&tag=' + data.Tag,
                        SelectCategory: function () {
                            var href = this.CategoryHref.replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
                            self.UpdatePosts(href);
                        },
                        SelectedCategoryHref: '/Blog/AllPosts?tag=' + data.Tag,
                        DeselectCategory: function () {
                            var href = this.SelectedCategoryHref.replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
                            self.UpdatePosts(href);
                        }
                    };

                    return post;
                })
            .ToArray();

        self.Posts(postsArray);

        self.TotalNumberOfPosts(data.TotalCount);
        self.NumberOfPostsInCurrentPage(postsArray.length);

        self.UrlSlug(data.UrlSlug);
        self.Category(data.Category);
        self.Tag(data.Tag);
        self.Archive(data.Archive);
        self.SearchTerm(data.SearchTerm);
        self.CurrPage(data.CurrPage);

        if (data.PrevPage || data.PrevPage === 0) {
            self.PrevPageUrl('/Blog/AllPosts/' + data.UrlSlug + '?category=' + data.Category + '&tag=' + data.Tag + '&cur=' + data.PrevPage + '&searchTerm=' + data.SearchTerm);
        }
        else {
            self.PrevPageUrl('');
        }

        if (data.NextPage) {
            self.NextPageUrl('/Blog/AllPosts/' + data.UrlSlug + '?category=' + data.Category + '&tag=' + data.Tag + '&cur=' + data.NextPage + '&searchTerm=' + data.SearchTerm);
        }
        else {
            self.NextPageUrl('');
        }

        window.scrollTo(0, 0);
    };

    self.GoToPrevPage = function () {
        var href = self.PrevPageUrl().replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
        self.UpdatePosts(href);
    }

    self.GoToNextPage = function () {
        var href = self.NextPageUrl().replace('/Blog/AllPosts', '/Blog/api/GetAllPosts');
        self.UpdatePosts(href);
    }
};
