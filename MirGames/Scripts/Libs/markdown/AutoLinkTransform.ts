module MarkdownTransform {
    export class AutoLinkTransform {
        private regularExpression: RegExp;

        constructor() {
            this.regularExpression = /(\[[^\]]*\]\()?((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?#%&~\-_]*)([\"'#!\(\)?, ><;])?(\))?/ig;
        }

        public transform(s: string): string {
            return s.replace(this.regularExpression, (substring?: string, p1?: string, p2?: string, p3?: string, p4?: string, p5?: string) => {
                if (p1 && substring.substr(-1, 1) == ')') {
                    return substring;
                }

                var link = ('(' + p2 + ')').replace('(www.', '(http://www.');

                return [p1 || '', '[', p2, ']', link, p5].join('');
            });
        }
    }

    export class NewLinesTextTransform {
        private regularExpression: RegExp;

        constructor() {
            this.regularExpression = /^[\w\<\>][^\n]*(\n+)/mig;
        }

        public transform(s: string): string {
            return s.replace(this.regularExpression, (substring?: string, p1?: string) => {
                if (p1 && p1.length > 1) {
                    return substring;
                }

                return substring.replace(/\s*$/, '  \n');
            });
        }
    }
}