namespace CutsceneML.Parser.AST.Impl;

public class OneOfParselet<T> : Parselet<T> {
	public required IParses<T>[] Subparselets { get; init; }

	public override T Read(Cursor cursor) {
		Cursor untouchedCursor = cursor.Clone();
		foreach (IParses<T> subparselet in Subparselets) {
			Cursor subcursor = untouchedCursor.Clone();
			try {
				T result = subparselet.Read(subcursor);

				cursor.Location = subcursor.Location;
				cursor.Errors = subcursor.Errors;

				return result;
			} catch (OperationCanceledException br) {
				if (subcursor.Location > cursor.Location) {
					cursor.Location = subcursor.Location;
					cursor.Errors.Clear();
				}

				if (subcursor.Location == cursor.Location) {
					cursor.Errors.AddRange(subcursor.Errors);
				}
			}
		}

		throw new OperationCanceledException("No valid branch found");
	}
}