package eu.nvna.tests;

import eu.nvna.helpers.ReflectedColor;
import eu.nvna.helpers.ValueSupplier;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.stream.Stream;

public class TestConstructors {
    @Test
    public void new_Color_EmptyCtor_ShouldBehaveNormally() throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException, NoSuchFieldException {
        var color = ReflectedColor.newInstance();
        Assertions.assertEquals(0, ReflectedColor.getFieldRValue(color));
        Assertions.assertEquals(0, ReflectedColor.getFieldGValue(color));
        Assertions.assertEquals(0, ReflectedColor.getFieldBValue(color));
    }

    @ParameterizedTest
    @MethodSource("supplyColors")
    public void new_Color_WithParameter_ShouldBehaveNormally(short r, short g, short b) throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException, NoSuchFieldException {
        var rgb = (long) (Math.pow(256, 2) * r + Math.pow(256, 1) * g + Math.pow(256, 0) * b);
        var color = ReflectedColor.newInstance(rgb);

        Assertions.assertEquals(r, ReflectedColor.getFieldRValue(color));
        Assertions.assertEquals(g, ReflectedColor.getFieldGValue(color));
        Assertions.assertEquals(b, ReflectedColor.getFieldBValue(color));
    }

    private static Stream<Arguments> supplyColors() {
        return ValueSupplier.supplyColors();
    }
}
