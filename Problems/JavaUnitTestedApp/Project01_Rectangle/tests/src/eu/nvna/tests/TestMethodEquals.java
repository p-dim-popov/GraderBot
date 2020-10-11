package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestMethodEquals {
    @Test
    public void checkIfEqualsBehavesAsExpected() throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var ctor = Rectangle.class.getConstructor(int.class, int.class, int.class, int.class);

        var ifThis = ctor.newInstance(1, 2, 3, 4);
        var that = ctor.newInstance(4,5,6,7);
        var impostor = ctor.newInstance(6,6,6,6);
        
        Assertions.assertTrue(ifThis.equals(that), "Equals does not return true for same rectangles");
        Assertions.assertFalse(ifThis.equals(impostor), "Equals returns true when comparing different rectangles");
    }
}
