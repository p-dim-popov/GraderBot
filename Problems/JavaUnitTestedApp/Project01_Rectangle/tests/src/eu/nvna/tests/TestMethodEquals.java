package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestMethodEquals {
    @Test
    public void checkIfEqualsBehavesAsExpected() throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var ctor = Rectangle.class.getConstructor(int.class, int.class, int.class, int.class);

        var main = ctor.newInstance(1, 2, 3, 4);
        var equal = ctor.newInstance(4,5,6,7);
        var notEqual = ctor.newInstance(6,6,6,6);

        var areEqual = Rectangle.class.getMethod("equals", Rectangle.class);

        Assertions.assertTrue((Boolean) areEqual.invoke(main, equal), "Equals does not return true for same rectangles");
        Assertions.assertFalse((Boolean) areEqual.invoke(main, notEqual), "Equals returns true when comparing different rectangles");
    }
}
