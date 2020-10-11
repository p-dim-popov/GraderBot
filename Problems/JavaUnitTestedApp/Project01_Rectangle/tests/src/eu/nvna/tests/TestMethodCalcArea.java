package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.stream.Stream;

public class TestMethodCalcArea {
    @ParameterizedTest
    @MethodSource("providerForCheckIfCalcAreaBehavesAsExpected")
    public void checkIfCalcAreaBehavesAsExpected(int x1, int y1, int x2, int y2, int area) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        var obj = Rectangle.class
                .getConstructor(int.class, int.class, int.class, int.class)
                .newInstance(x1, y1, x2, y2);
        Assertions.assertEquals(area, obj.calcArea());
    }

    private static Stream<Arguments> providerForCheckIfCalcAreaBehavesAsExpected() {
        return Stream.of(
                Arguments.of(1, 2, 3, 4, 4),
                Arguments.of(34, 34, 62, 7, 756),
                Arguments.of(-123, 41, -3, 0, 4920),
                Arguments.of(0, 0, 234, 0, 0),
                Arguments.of(1, -1, 1, -1, 0),
                Arguments.of(66, 66, 66, 66, 0)
        );

//        Results from original class
//        Stream.of(
//                java.util.List.of(1, 2, 3, 4),
//                java.util.List.of(34, 34, 62, 7),
//                java.util.List.of(-123, 41, -3, 0),
//                java.util.List.of(0, 0, 234, 0),
//                java.util.List.of(1, -1, 1, -1),
//                java.util.List.of(66, 66, 66, 66))
//                .map(l -> new Rectangle(l.get(0), l.get(1), l.get(2), l.get(3)))
//                .forEach(r -> System.out.println(r.calcArea()));
    }
}
