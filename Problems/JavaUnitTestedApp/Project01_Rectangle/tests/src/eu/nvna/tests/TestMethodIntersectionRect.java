package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.stream.Stream;

public class TestMethodIntersectionRect {
    @ParameterizedTest
    @MethodSource("providerForCheckIfIntersectionRectBehavesAsExpected")
    public void checkIfIntersectionRectBehavesAsExpected(int x1, int y1, int x2, int y2) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var ctor = Rectangle.class
                .getConstructor(int.class, int.class, int.class, int.class);

        var left = ctor
                .newInstance(x1, y1, x2, y2);
        var right = ctor
                .newInstance(y1, x1, y2, x2);

        int leftIX1 = Math.min(x1, x2);
        int leftIY1 = Math.min(y1, y2);
        int leftIX2 = Math.max(x1, x2);
        int leftIY2 = Math.max(y1, y2);

        int rightIX1 = Math.min(y1, y2);
        int rightIY1 = Math.min(x1, x2);
        int rightIX2 = Math.max(y1, y2);
        int rightIY2 = Math.max(x1, x2);

        var fields = Arrays.asList(Rectangle.class
                .getDeclaredFields());
        fields.forEach(f -> f.setAccessible(true));

        var fieldIX1 = fields.stream().filter(f -> f.getName().equals("iX1")).findFirst().get();
        var fieldIY1 = fields.stream().filter(f -> f.getName().equals("iY1")).findFirst().get();
        var fieldIX2 = fields.stream().filter(f -> f.getName().equals("iX2")).findFirst().get();
        var fieldIY2 = fields.stream().filter(f -> f.getName().equals("iY2")).findFirst().get();

        var expectedUnion = ctor.newInstance(Math.max(leftIX1, rightIX1),
                Math.max(leftIY1, rightIY1),
                Math.min(leftIX2, rightIX2),
                Math.min(leftIY2, rightIY2));
        if ((int) fieldIX1.get(expectedUnion) > (int) fieldIX2.get(expectedUnion)) {
            fieldIX1.set(expectedUnion, 0);
            fieldIX2.set(expectedUnion, 0);
        }
        if ((int) fieldIY1.get(expectedUnion) > (int) fieldIY2.get(expectedUnion)) {
            fieldIY1.set(expectedUnion, 0);
            fieldIY2.set(expectedUnion, 0);
        }

        var getUnion = Rectangle.class.getMethod("intersectionRect", Rectangle.class);
        var actualUnion = getUnion.invoke(left, right);

        Assertions.assertEquals(fieldIX1.get(expectedUnion), fieldIX1.get(actualUnion));
        Assertions.assertEquals(fieldIY1.get(expectedUnion), fieldIY1.get(actualUnion));
        Assertions.assertEquals(fieldIX2.get(expectedUnion), fieldIX2.get(actualUnion));
        Assertions.assertEquals(fieldIY2.get(expectedUnion), fieldIY2.get(actualUnion));
    }

    private static Stream<Arguments> providerForCheckIfIntersectionRectBehavesAsExpected() {
        return Stream.of(
                Arguments.of(1, 2, 3, 4),
                Arguments.of(34, 34, 62, 7),
                Arguments.of(-123, 41, -3, 0),
                Arguments.of(0, 0, 234, 0),
                Arguments.of(1, -1, 1, -1),
                Arguments.of(66, 66, 66, 66)
        );
    }

}
