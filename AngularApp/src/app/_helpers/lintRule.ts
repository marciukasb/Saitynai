import * as ts from "typescript";
import * as Lint from "tslint";

export class Rule extends Lint.Rules.AbstractRule {
    public apply(sourceFile: ts.SourceFile) : Lint.RuleFailure[] {
        return this.applyWithWalker(new FirstIsLower(sourceFile, this.getOptions()));
    }
}
class FirstIsLower extends Lint.RuleWalker {
    public visitPropertyDeclaration(node: ts.PropertyDeclaration) : void {
        const name= node.name.getText();
        if(!this.validateMemberPropertyOrMethod(name)) {
            this.addFailure(this.createFailure(node.getStart(), node.getWidth(), "First letter of '" + name + "' should be a lower case letter"));
        }
        super.visitPropertyDeclaration(node);
    }
    private validateMemberPropertyOrMethod(name: string) : boolean {
        return name.charAt(0) === name.charAt(0).toLowerCase();
    }
}