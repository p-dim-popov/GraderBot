package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.stream.Stream;

public class TestMethodToString {
    @ParameterizedTest
    @MethodSource("providerForCheckIfToStringBehavesAsExpected")
    public void checkIfToStringBehavesAsExpected(int x1, int y1, int x2, int y2) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var obj = Rectangle.class
                .getConstructor(int.class, int.class, int.class, int.class)
                .newInstance(x1, y1, x2, y2);

        Assertions.assertEquals("{ iX1: " + Math.min(x1, x2) +
                        ", iY1: " + Math.min(y1, y2) +
                        ", iX2: " + Math.max(x1, x2) +
                        ", iY2: " + Math.max(y1, y2) +
                        " }",
                obj.toString());
    }

    private static Stream<Arguments> providerForCheckIfToStringBehavesAsExpected() {
        return Stream.of(
                Arguments.of(1, 2, 3, 4, 4),
                Arguments.of(34, 34, 62, 7, 756),
                Arguments.of(-123, 41, -3, 0, 4920),
                Arguments.of(0, 0, 234, 0, 0),
                Arguments.of(1, -1, 1, -1, 0),
                Arguments.of(66, 66, 66, 66, 0)
        );
    }
}
