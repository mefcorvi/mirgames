var MarkdownTransform;
(function (MarkdownTransform) {
    var AutoLinkTransform = (function () {
        function AutoLinkTransform() {
            this.regularExpression = /(\[[^\]]*\]\()?((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?#%&~\-_]*)([\"'#!\(\)?, ><;])?(\))?/ig;
        }
        AutoLinkTransform.prototype.transform = function (s) {
            return s.replace(this.regularExpression, function (substring, p1, p2, p3, p4, p5) {
                if (p1 && substring.substr(-1, 1) == ')') {
                    return substring;
                }

                var link = ('(' + p2 + ')').replace('(www.', '(http://www.');

                return [p1 || '', '[', p2, ']', link, p5].join('');
            });
        };
        return AutoLinkTransform;
    })();
    MarkdownTransform.AutoLinkTransform = AutoLinkTransform;

    var NewLinesTextTransform = (function () {
        function NewLinesTextTransform() {
            this.regularExpression = /^[\w\<\>][^\n]*(\n+)/mig;
        }
        NewLinesTextTransform.prototype.transform = function (s) {
            return s.replace(this.regularExpression, function (substring, p1) {
                if (p1 && p1.length > 1) {
                    return substring;
                }

                return substring.replace(/\s*$/, '  \n');
            });
        };
        return NewLinesTextTransform;
    })();
    MarkdownTransform.NewLinesTextTransform = NewLinesTextTransform;
})(MarkdownTransform || (MarkdownTransform = {}));
//# sourceMappingURL=AutoLinkTransform.js.map
