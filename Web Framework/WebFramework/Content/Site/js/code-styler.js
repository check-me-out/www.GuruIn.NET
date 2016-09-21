function init() {

    $(".blog-content pre").each(function () {
        var value = $(this).html();
        value = value.replace(/<span class/g, 'lkhgr82tvnzligy');

        for (var i = 0; i < KeywordList.length; i++) {
            value = value.replace(new RegExp("\\b" + KeywordList[i] + "\\b", 'g'), '<span class=\"keyword\">' + KeywordList[i] + '</span>');
        }

        for (var i = 0; i < TypeList.length; i++) {
            value = value.replace(new RegExp("\\b" + TypeList[i] + "\\b", 'g'), '<span class=\"type\">' + TypeList[i] + '</span>');
        }

        var comments = value.split('//');
        if (comments.length > 1) {
            for (var i = 1; i < comments.length; i++) {
                comments[i] = '<span class="comment">//' + comments[i];
                var eoc = comments[i].indexOf('\n');
                if (eoc > 0)
                    comments[i] = comments[i].substring(0, eoc) + '</span>' + comments[i].substring(eoc, comments[i].length);
                else
                    comments[i] = comments[i] + '</span>';
            }

            value = comments.join("");
        }

        value = value.replace(/lkhgr82tvnzligy/g, '<span class');
        $(this).html(value);
    });
}

RegisterOnload(init);


var KeywordList = [
    "class", "interface", "public", "private", "internal", "static", "readonly",
    "if", "foreach", "var", "new", "base", "return",
    "string", "int", "bool", "void", "null", "true", "false", 
    "override", "virtual", "try", "catch", "finally", "using",
];

var TypeList = [
    "AuthorizeAttribute",
    "AttributeUsage",
    "AttributeTargets",
    "Controller",
    "Exception",
    "IEnumerable",
    "List",
    "ActionResult",
    "ActionFilterAttribute",
    "ActionExecutingContext",
];
