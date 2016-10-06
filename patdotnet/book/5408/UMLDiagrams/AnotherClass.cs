namespace UMLDiagrams
{

	public class ControlData {

	}
	public interface IComponent {
		ControlData GenericType;
	}

	public interface IComponentStreaming : IComponent {

	}

	public class Chain {
		IComponent _reference;
	}
}
