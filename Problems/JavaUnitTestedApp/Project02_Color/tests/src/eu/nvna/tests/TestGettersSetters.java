package eu.nvna.tests;

import eu.nvna.helpers.ReflectedColor;
import eu.nvna.helpers.ValueSupplier;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.stream.Stream;

public class TestGettersSetters {
    @ParameterizedTest
    @MethodSource("supplyColors")
    public void getters_ShouldBehaveNormally(short r, short g, short b) throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException {
        var rgb = (long) (Math.pow(256, 2) * r + Math.pow(256, 1) * g + Math.pow(256, 0) * b);
        var color = ReflectedColor.newInstance(rgb);

        Assertions.assertEquals(r, ReflectedColor.getGetterRValue().invoke(color));
        Assertions.assertEquals(g, ReflectedColor.getGetterGValue().invoke(color));
        Assertions.assertEquals(b, ReflectedColor.getGetterBValue().invoke(color));
    }

    @ParameterizedTest
    @MethodSource("supplyColors")
    public void setters_ShouldBehaveNormally(short r, short g, short b) throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException, NoSuchFieldException {
        var color = ReflectedColor.newInstance();

        ReflectedColor.getSetterSetRValue().invoke(color, r);
        ReflectedColor.getSetterSetGValue().invoke(color, g);
        ReflectedColor.getSetterSetBValue().invoke(color, b);

        Assertions.assertEquals(r, ReflectedColor.getFieldRValue(color));
        Assertions.assertEquals(g, ReflectedColor.getFieldGValue(color));
        Assertions.assertEquals(b, ReflectedColor.getFieldBValue(color));

        Assertions.assertEquals(
                (long) (Math.pow(256, 2) * r + Math.pow(256, 1) * g + Math.pow(256, 0) * b),
                ReflectedColor.getFieldRGBValue(color));
    }
    private static Stream<Arguments> supplyColors(){
        return ValueSupplier.supplyColors();
    }

}
