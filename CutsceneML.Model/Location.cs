namespace CutsceneML.Model;

public record class Location(
	string File,
	int Line,
	int Character
) : IComparable<Location> {
	public int CompareTo(Location? other) {
		if (ReferenceEquals(this, other)) {
			return 0;
		}

		if (other is null) {
			return 1;
		}

		int fileComparison =
			string.Compare(File, other.File, StringComparison.Ordinal);
		if (fileComparison != 0) {
			return fileComparison;
		}

		int lineComparison = Line.CompareTo(other.Line);
		if (lineComparison != 0) {
			return lineComparison;
		}

		return Character.CompareTo(other.Character);
	}

	public override string ToString() => $"({File}:{Line}:{Character})";
}