using System.Diagnostics;

using CutsceneML.Model;

namespace CutsceneML.Parser.AST;

public closed class Tree : ITreeNode {
	public abstract ITreeNode[] Children { get; }
	public Location Start => Children[0].Start;
	public Location End => Children[^1].End;

	public void AssertContinuous() {
		IEnumerable<Location> locs = Children.SelectMany(it =>
			(IEnumerable<Location>)[it.Start, it.End]);

		Location? previousLocation = null;

		foreach (Location location in locs) {
			if (previousLocation == null) {
				previousLocation = location;
				continue;
			}

			int delta = location.CompareTo(previousLocation);

			Debug.Assert(delta >= 0,
				"The AST node " + ToString() + " is not continuous, " +
				location + " comes before " + previousLocation);

			previousLocation = location;
		}

		foreach (ITreeNode child in Children) {
			if (child is Tree tree) {
				tree.AssertContinuous();
			}
		}
	}

	public override string ToString() =>
		$"{GetType().Name} ({string.Join(", ", Children)})";
}