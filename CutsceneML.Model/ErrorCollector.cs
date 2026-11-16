namespace CutsceneML.Model;

public class ErrorCollector {
	public List<Error> Errors = [];

	public void AssertNoErrors() {
		if (Errors.Count == 0) {
			return;
		}

		throw new Exception("Unexpected errors: " + string.Join("\n", Errors));
	}
}