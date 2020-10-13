package eu.nvna.tests;

import eu.nvna.helpers.ReflectedColor;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestCompareTo {
    @Test
    public void compareTo_ShouldBehaveNormally() throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException {
        var modelColor = (long) (Math.pow(256, 2) * 134 + Math.pow(256, 1) * 123 + Math.pow(256, 0) * 12);
        var model = ReflectedColor.newInstance(modelColor);
        var sameModel = ReflectedColor.newInstance(modelColor);
        var smaller = ReflectedColor.newInstance((long) (Math.pow(256, 2) * 34 + Math.pow(256, 1) * 0 + Math.pow(256, 0) * 85));
        var bigger = ReflectedColor.newInstance((long) (Math.pow(256, 2) * 234 + Math.pow(256, 1) * 5 + Math.pow(256, 0) * 0));

        Assertions.assertEquals(0, ReflectedColor.compare(model, model), "Comparing same objects does not return 0!");
        Assertions.assertEquals(0, ReflectedColor.compare(model, sameModel), "Comparing same objects does not return 0!");
        Assertions.assertEquals(1, ReflectedColor.compare(model, smaller), "Comparing with smaller object does not return 1!");
        Assertions.assertEquals(-1, ReflectedColor.compare(model, bigger), "Comparing with bigger object does not return -1!");
    }
}
