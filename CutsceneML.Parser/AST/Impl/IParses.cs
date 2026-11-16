namespace CutsceneML.Parser.AST.Impl;

public interface IParses<out T> {
	T Read(Cursor cursor);
}