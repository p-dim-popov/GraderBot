package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import org.junit.jupiter.params.provider.ValueSource;

import java.lang.reflect.*;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class TestGetters {
    private final List<Method> _getters =
            Arrays.stream(Rectangle.class.getDeclaredMethods())
                    .filter(m -> m.getName().matches("get.*"))
                    .collect(Collectors.toList());


    @ParameterizedTest
    @MethodSource("providerForGettersShouldBehaveAsExpected")
    public void gettersShouldBehaveAsExpected(String fieldName, int[] testValues) throws NoSuchMethodException, NoSuchFieldException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var obj = Rectangle.class.getConstructor().newInstance();

        String getterName = "get" + fieldName
                .replaceFirst("^.", String
                        .valueOf(fieldName.charAt(0))
                        .toUpperCase());

        var getter = _getters
                .stream()
                .filter(g -> g.getName().equals(getterName))
                .findFirst()
                .get();
        var field = obj.getClass()
                .getDeclaredField(fieldName);

        Arrays.stream(testValues)
                .forEach(v -> {
                    field.setAccessible(true);
                    try {
                        field.set(obj, v);
                        Assertions.assertEquals(v, getter.invoke(obj));
                    } catch (IllegalAccessException | InvocationTargetException e) {
                        Assertions.fail(e);
                    }
                });
    }

    private static Stream<Arguments> providerForGettersShouldBehaveAsExpected() {
        var testValues = new int[]{1, 2, 3, 99, 88, 77, -1, -2, -1234};
        return Stream.of(
                Arguments.of("iX1", testValues),
                Arguments.of("iY1", testValues),
                Arguments.of("iX2", testValues),
                Arguments.of("iY2", testValues)
        );
    }
}
