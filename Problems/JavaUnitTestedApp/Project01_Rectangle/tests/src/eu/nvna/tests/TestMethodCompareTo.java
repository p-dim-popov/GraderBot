package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestMethodCompareTo {
    @Test
    public void checkIfCompareToBehavesAsExpected() throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var ctor = Rectangle.class.getConstructor(int.class, int.class, int.class, int.class);

        var small = ctor.newInstance(-1, -1, 1, 1);
        var medium = ctor.newInstance(-2,2,5,5);
        var big = ctor.newInstance(-23, -211, 765, 64);

        var compareTo = Rectangle.class.getMethod("compareTo", Object.class);

        Assertions.assertEquals(1, compareTo.invoke(medium, small));
        Assertions.assertEquals(0, compareTo.invoke(medium, medium));
        Assertions.assertEquals(-1, compareTo.invoke(medium, big ));
    }
}
