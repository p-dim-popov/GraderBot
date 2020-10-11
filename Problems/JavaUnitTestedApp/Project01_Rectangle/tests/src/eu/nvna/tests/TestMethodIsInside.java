package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.ArgumentsSource;
import org.junit.jupiter.params.provider.MethodSource;
import org.junit.jupiter.params.provider.ValueSource;

import java.lang.reflect.Array;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import java.util.stream.Stream;

public class TestMethodIsInside {
//    @ParameterizedTest
//    @MethodSource("providerForCheckIfIsInsideBehavesAsExpected")
//    public void checkIfIsInsideBehavesAsExpected(int x1, int y1, int x2, int y2) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
//        var obj = Rectangle.class
//                .getConstructor(int.class, int.class, int.class, int.class)
//                .newInstance(x1, y1, x2, y2);
//
//        var isInside = Rectangle.class.getMethod("isInside", int.class, int.class);
//
//        Assertions.assertTrue(isInside.invoke(obj, ));
//    }
//
//    private static Stream<Arguments> providerForCheckIfIsInsideBehavesAsExpected() {
//        return Stream.of(
//                Arguments.of(1, 2, 3, 4, 4),
//                Arguments.of(34, 34, 62, 7, 756),
//                Arguments.of(-123, 41, -3, 0, 4920),
//                Arguments.of(0, 0, 234, 0, 0),
//                Arguments.of(1, -1, 1, -1, 0),
//                Arguments.of(66, 66, 66, 66, 0)
//        );
//    }
}