namespace CutsceneML.Parser.AST;

public interface IParses<out T> {
	T Read(Cursor cursor);
}