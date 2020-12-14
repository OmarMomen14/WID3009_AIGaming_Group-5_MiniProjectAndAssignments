package pacman.entries.pacman.bt.composite;

import pacman.entries.pacman.bt.TreeNode;

import java.util.ArrayList;

public abstract class CompositeNode extends TreeNode {
    protected ArrayList<TreeNode> children;

    protected CompositeNode(String name) {
        super(name);
        children = new ArrayList<>();
    }

    public void addChild(TreeNode child) {
        children.add(child);
    }
}
