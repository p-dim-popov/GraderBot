package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Modifier;
import java.util.Arrays;
import java.util.stream.Stream;

public class TestConstructors {
    private final Constructor<?>[] _constructors = Rectangle.class.getDeclaredConstructors();

    @Test
    public void checkConstructorsCount() {
        Assertions.assertTrue(_constructors.length >= 2,
                "Constructors count was below required count!");
    }

    @Test
    public void checkConstructorsModifiers() {
        Assertions.assertTrue(Arrays.stream(_constructors)
                        .map(Constructor::getModifiers)
                        .allMatch(Modifier::isPublic),
                "Some of the constructors were not public");
    }

    @ParameterizedTest
    @MethodSource("provideArgsForExplicitConstructorsShouldBehaveAsExpected")
    public void explicitConstructorsShouldBehaveAsExpected(int x1, int y1, int x2, int y2)
            throws IllegalAccessException, InvocationTargetException, InstantiationException {
        var ctor = Arrays.stream(_constructors)
                .filter(c -> c.getParameterCount() == 4)
                .findFirst();
        Assertions.assertTrue(ctor.isPresent(),
                "No constructor with 4 parameters was found!");
        Assertions.assertTrue(Arrays.stream(ctor.get()
                        .getParameterTypes())
                        .allMatch(p -> p.equals(int.class)),
                "Some or all of the constructor parameters were not of correct type");

        var obj = ctor.get().newInstance(x1, y1, x2, y2);

        try {
            var fields = Arrays.asList(obj.getClass()
                    .getDeclaredFields());
            fields.forEach(f -> f.setAccessible(true));

            var x1_get = fields
                    .stream()
                    .filter(f -> f.getName().equals("iX1"))
                    .findFirst()
                    .get()
                    .get(obj);
            var y1_get = fields
                    .stream()
                    .filter(f -> f.getName().equals("iY1"))
                    .findFirst()
                    .get()
                    .get(obj);
            var x2_get = fields
                    .stream()
                    .filter(f -> f.getName().equals("iX2"))
                    .findFirst()
                    .get()
                    .get(obj);
            var y2_get = fields
                    .stream()
                    .filter(f -> f.getName().equals("iY2"))
                    .findFirst()
                    .get()
                    .get(obj);

            Assertions.assertEquals(Math.min(x1, x2), x1_get);
            Assertions.assertEquals(Math.min(y1, y2), y1_get);
            Assertions.assertEquals(Math.max(x1, x2), x2_get);
            Assertions.assertEquals(Math.max(y1, y2), y2_get);

        } catch (IllegalAccessException e) {
            Assertions.fail(e);
        }
    }

    private static Stream<Arguments> provideArgsForExplicitConstructorsShouldBehaveAsExpected() {
        return Stream.of(
                Arguments.of(0, 1, 2, 3, 4),
                Arguments.of(5, 6, 7, 8),
                Arguments.of(99, 88, 77, 66),
                Arguments.of(-1,2,-5,34)
        );
    }
}
