using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEditor;

// Use InputBindingComposite<TValue> as a base class for a composite that returns
// values of type TValue.
// NOTE: It is possible to define a composite that returns different kinds of values
//       but doing so requires deriving directly from InputBindingComposite.
#if UNITY_EDITOR
[InitializeOnLoad] // Automatically register in editor.
#endif
// Determine how GetBindingDisplayString() formats the composite by applying
// the  DisplayStringFormat attribute.
[DisplayStringFormat("{firstPart}+{secondPart}")]
public class CustomNumbersComposite : InputBindingComposite<float>
{
    // Each part binding is represented as a field of type int and annotated with
    // InputControlAttribute. Setting "layout" restricts the controls that
    // are made available for picking in the UI.
    //
    // On creation, the int value is set to an integer identifier for the binding
    // part. This identifier can read values from InputBindingCompositeContext.
    // See ReadValue() below.
    [InputControl(layout = "Button")]
    public int numberOne;

    [InputControl(layout = "Button")]
    public int numberTwo;
    
    [InputControl(layout = "Button")]
    public int numberThree;
    
    [InputControl(layout = "Button")]
    public int numberFour;

    [InputControl(layout = "Button")]
    public int numberFive;
    
    [InputControl(layout = "Button")]
    public int numberSix;
    
    [InputControl(layout = "Button")]
    public int numberSeven;

    [InputControl(layout = "Button")]
    public int numberEight;
    
    [InputControl(layout = "Button")]
    public int numberNine;

    // Any public field that is not annotated with InputControlAttribute is considered
    // a parameter of the composite. This can be set graphically in the UI and also
    // in the data (e.g. "custom(floatParameter=2.0)").
    //public float floatParameter;
    //public bool boolParameter;

    // This method computes the resulting input value of the composite based
    // on the input from its part bindings.
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        var numberOneValue = context.ReadValue<float>(numberOne);
        var numberTwoValue = context.ReadValue<float>(numberTwo);
        var numberThreeValue = context.ReadValue<float>(numberThree);
        var numberFourValue = context.ReadValue<float>(numberFour);
        var numberFiveValue = context.ReadValue<float>(numberFive);
        var numberSixValue = context.ReadValue<float>(numberSix);
        var numberSevenValue = context.ReadValue<float>(numberSeven);
        var numberEightValue = context.ReadValue<float>(numberEight);
        var numberNineValue = context.ReadValue<float>(numberNine);
        
        if (numberOneValue == 1)
        {
            return 1;
        }
        else if (numberTwoValue == 1)
        {
            return 2;
        }
        else if (numberThreeValue == 1)
        {
            return 3;
        }
        else if (numberFourValue == 1)
        {
            return 4;
        }
        else if (numberFiveValue == 1)
        {
            return 5;
        }
        else if (numberSixValue == 1)
        {
            return 6;
        }
        else if (numberSevenValue == 1)
        {
            return 7;
        }
        else if (numberEightValue == 1)
        {
            return 8;
        }
        else if (numberNineValue == 1)
        {
            return 9;
        }
        else return default;

        //... do some processing and return value
    }

    // This method computes the current actuation of the binding as a whole.
    /*public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        // Compute normalized [0..1] magnitude value for current actuation level.
    }*/

    static CustomNumbersComposite()
    {
        // Can give custom name or use default (type name with "Composite" clipped off).
        // Same composite can be registered multiple times with different names to introduce
        // aliases.
        //
        // NOTE: Registering from the static constructor using InitializeOnLoad and
        //       RuntimeInitializeOnLoadMethod is only one way. You can register the
        //       composite from wherever it works best for you. Note, however, that
        //       the registration has to take place before the composite is first used
        //       in a binding. Also, for the composite to show in the editor, it has
        //       to be registered from code that runs in edit mode.
        InputSystem.RegisterBindingComposite<CustomNumbersComposite>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {} // Trigger static constructor.
}
