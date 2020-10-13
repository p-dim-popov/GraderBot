package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.function.BiFunction;
import java.util.stream.Stream;

public class TestMethodIsInside {
    @ParameterizedTest
    @MethodSource("providerForCheckIfIsInsideBehavesAsExpected")
    public void checkIfIsInsideBehavesAsExpected(int x1, int y1, int x2, int y2, int[][] points) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var obj = Rectangle.class
                .getConstructor(int.class, int.class, int.class, int.class)
                .newInstance(x1, y1, x2, y2);

        int iX1 = Math.min(x1, x2);
        int iY1 = Math.min(y1, y2);
        int iX2 = Math.max(x1, x2);
        int iY2 = Math.max(y1, y2);

        BiFunction<Integer, Integer, Boolean> isInsideExpected = (x, y) -> ((x > iX1) && (x < iX2) && (y > iY1) && (y < iY2));

        var isInsideActual = Rectangle.class.getMethod("isInside", int.class, int.class);

        Arrays.stream(points).forEach(p -> {
            try {
                Assertions.assertEquals((Boolean) isInsideActual.invoke(obj, p[0], p[1]), isInsideExpected.apply(p[0], p[1]));
            } catch (IllegalAccessException | InvocationTargetException e) {
                Assertions.fail(e);
            }
        });
    }

    private static Stream<Arguments> providerForCheckIfIsInsideBehavesAsExpected() {
        var points = Stream.of(
                new int[]{1, 2},
                new int[]{1, 2},
                new int[]{-6, 2},
                new int[]{0, 0},
                new int[]{12, -2},
                new int[]{3, 1}
        ).toArray(int[][]::new);
        return Stream.of(
                Arguments.of(1, 2, 3, 4, points),
                Arguments.of(34, 34, 62, 7, points),
                Arguments.of(-123, 41, -3, 0, points),
                Arguments.of(0, 0, 234, 0, points),
                Arguments.of(1, -1, 1, -1, points),
                Arguments.of(66, 66, 66, 66, points)
        );
    }
}