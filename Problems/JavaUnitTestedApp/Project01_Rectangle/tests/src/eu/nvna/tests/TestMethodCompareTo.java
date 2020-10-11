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

        Assertions.assertEquals(1, medium.compareTo(small));
        Assertions.assertEquals(0, medium.compareTo(medium));
        Assertions.assertEquals(-1, medium.compareTo(big));
    }
}
