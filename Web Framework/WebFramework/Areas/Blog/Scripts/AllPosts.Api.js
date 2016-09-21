// tell JSLint to suppress errors relating to objects declared elsewhere  (not strictly essential if you don't use JSLint)
/*global $, allPostsApi, allPostsMvcViewModel */

var allPostsApi = allPostsApi || {};
var allPostsMvcViewModel = allPostsMvcViewModel || null;

allPostsApi.client = (function (allPostsApiEndpoint, selectedPostApiEndpoint) {

	var
    getAllPosts = function getAllPosts(slug, category, tag, cur, archive, searchTerm) {
        var endpoint = allPostsApiEndpoint + "/GetAllPosts/" + slug + '?category=' + category + '&tag=' + tag + '&cur=' + cur + '&archive=' + archive + '&searchTerm=' + searchTerm;
    	return $.ajax({
    	    url: endpoint,
    	    type: "GET",
    	    dataType: 'json',
    		async: true
    	});
    },

    invoke = function invoke(url) {
    	return $.ajax({
    		url: url,
    		type: "GET",
    		dataType: 'json',
    		async: true
    	});
    },

    completeManualSafeCount = function completeManualSafeCount(fullCountObject) {

    	return $.ajax({
    		url: allPostsApiEndpoint + "/CompleteManualSafeCount",
    		type: "POST",
    		contentType: "application/json; charset=utf-8",
    		dataType: 'json',
    		async: false,
    		data: JSON.stringify({ FullCountJsonObject: fullCountObject })
    	});
    };

	return {
	    getAllPosts: getAllPosts,
	    invoke: invoke
	};

}(allPostsMvcViewModel.AllPostsApiEndpoint, allPostsMvcViewModel.SelectedPostApiEndpoint));
