package eu.nvna.tests;

import eu.nvna.helpers.ReflectedColor;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestToString {
    @Test
    public void toString_ShouldBehaveNormally() throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException, NoSuchFieldException {
        var color = ReflectedColor
                .newInstance((long) (Math.pow(256, 2) * 234 + Math.pow(256, 1) * 123 + Math.pow(256, 0) * 12));

        Assertions.assertEquals("Color{" +
                "RValue=" + ReflectedColor.getFieldRValue(color) +
                ", GValue=" + ReflectedColor.getFieldGValue(color) +
                ", BValue=" + ReflectedColor.getFieldBValue(color) +
                ", RGB=" + ReflectedColor.getFieldRGBValue(color) +
                '}',
                ReflectedColor.toString(color));
    }
}
