package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class TestXYTranslationMethods {
    @ParameterizedTest
    @MethodSource("providerForCheckIfTranslationMethodsBehaveAsExpected")
    public void checkIfTranslationMethodsBehaveAsExpected(int x1, int y1, int x2, int y2, int[] translations) throws NoSuchMethodException {
        var ctor = Rectangle.class.getConstructor(int.class, int.class, int.class, int.class);

        var fields = Arrays.stream(Rectangle.class.getDeclaredFields())
                .filter(f -> f.getName().startsWith("i"))
                .collect(Collectors
                        .toList());
        fields.forEach(f -> f.setAccessible(true));
        var fieldIX1 = fields.stream().filter(f -> f.getName().equals("iX1")).findFirst().get();
        var fieldIY1 = fields.stream().filter(f -> f.getName().equals("iY1")).findFirst().get();
        var fieldIX2 = fields.stream().filter(f -> f.getName().equals("iX2")).findFirst().get();
        var fieldIY2 = fields.stream().filter(f -> f.getName().equals("iY2")).findFirst().get();

        var translateX = Rectangle.class.getMethod("translateX", int.class);
        var translateY = Rectangle.class.getMethod("translateY", int.class);
        var translateXY = Rectangle.class.getMethod("translateXY", int.class);

        int iX1 = Math.min(x1, x2);
        int iY1 = Math.min(y1, y2);
        int iX2 = Math.max(x1, x2);
        int iY2 = Math.max(y1, y2);

        Arrays.stream(translations)
                .forEach(t -> {
                    try {
                        var obj = ctor.newInstance(x1, y1, x2, y2);
                        translateX.invoke(obj, t);
                        translateY.invoke(obj, t);

                        Assertions.assertEquals(fieldIX1.get(obj), iX1 + t);
                        Assertions.assertEquals(fieldIY1.get(obj), iY1 + t);
                        Assertions.assertEquals(fieldIX2.get(obj), iX2 + t);
                        Assertions.assertEquals(fieldIY2.get(obj), iY2 + t);

                        translateXY.invoke(obj, t);

                        Assertions.assertEquals(fieldIX1.get(obj), iX1 + 2 * t);
                        Assertions.assertEquals(fieldIY1.get(obj), iY1 + 2 * t);
                        Assertions.assertEquals(fieldIX2.get(obj), iX2 + 2 * t);
                        Assertions.assertEquals(fieldIY2.get(obj), iY2 + 2 * t);
                    } catch (InstantiationException | IllegalAccessException | InvocationTargetException e) {
                        Assertions.fail(e);
                    }
                });
    }

    private static Stream<Arguments> providerForCheckIfTranslationMethodsBehaveAsExpected() {
        var translations = new int[]{1, 2, 3, 99, 88, 77, -1, -2, -1234};
        return Stream.of(
                Arguments.of(1, 2, 3, 4, translations),
                Arguments.of(34, 34, 62, 7, translations),
                Arguments.of(-123, 41, -3, 0, translations),
                Arguments.of(0, 0, 234, 0, translations),
                Arguments.of(1, -1, 1, -1, translations),
                Arguments.of(66, 66, 66, 66, translations)
        );
    }
}
