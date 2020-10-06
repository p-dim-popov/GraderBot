package eu.nvna.tests;

import eu.nvna.classes.Point;
import eu.nvna.classes.Rectangle;
import org.junit.Test;

import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.util.Arrays;
import java.util.function.Supplier;
import java.util.stream.Stream;

import static org.junit.Assert.*;


public class TestFunctionsExistence {

    @Test
    public void checkIfPointFunctionsExist() {
        Supplier<Stream<Method>> functionsSupplier =
                () -> Arrays.stream(Point.class.getDeclaredMethods());

        var methodGetX = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("getX"))
                .findFirst();
        var methodGetY = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("getY"))
                .findFirst();

        var methodSetX = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("setX"))
                .findFirst();
        var methodSetY = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("setY"))
                .findFirst();

        assertEquals(4, functionsSupplier.get().count());
        assertTrue("getter for x not found", methodGetX.isPresent());
        assertTrue("getter for y not found", methodGetY.isPresent());
        assertTrue("setter for x not found", methodSetX.isPresent());
        assertTrue("setter for y not found", methodSetY.isPresent());


        assertTrue("getter for x is not public", Modifier.isPublic(methodGetX.get().getModifiers()));
        assertTrue("getter for y is not public", Modifier.isPublic(methodGetY.get().getModifiers()));

        assertTrue("setter for x is not public", Modifier.isPublic(methodSetX.get().getModifiers()));
        assertTrue("setter for y is not public", Modifier.isPublic(methodSetY.get().getModifiers()));


        assertTrue(methodGetX.get().getName() + "is not of type" + int.class.getSimpleName(),
                methodGetX.get()
                        .getReturnType()
                        .isAssignableFrom(int.class));
        assertTrue(methodGetY.get().getName() + "is not of type" + int.class.getSimpleName(),
                methodGetY.get()
                        .getReturnType()
                        .isAssignableFrom(int.class));

        assertTrue(methodSetX.get().getName() + "is not of type" + void.class.getSimpleName(),
                methodSetX.get()
                        .getReturnType()
                        .isAssignableFrom(void.class));
        assertTrue(methodSetY.get().getName() + "is not of type" + void.class.getSimpleName(),
                methodSetY.get()
                        .getReturnType()
                        .isAssignableFrom(void.class));

        assertEquals(1, methodSetX.get().getParameterCount());
        assertEquals(1, methodSetY.get().getParameterCount());

        assertEquals(int.class,Arrays.stream(methodSetX.get().getParameterTypes()).findFirst().get());

        assertEquals(int.class, Arrays.stream(methodSetY.get().getParameterTypes()).findFirst().get());
    }

    @Test
    public void checkIdRectangleFunctionsExist() {
        Supplier<Stream<Method>> functionsSupplier =
                () -> Arrays.stream(Rectangle.class.getDeclaredMethods());

        var methodGetTopLeft = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("getTopLeft"))
                .findFirst();
        var methodGetBottomRight = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("getBottomRight"))
                .findFirst();

        var methodSetTopLeft = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("setTopLeft"))
                .findFirst();
        var methodSetBottomRight = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("setBottomRight"))
                .findFirst();

        var methodContains = functionsSupplier
                .get()
                .filter(m -> m.getName().equals("contains"))
                .findFirst();

        assertEquals(5, functionsSupplier.get().count());
        assertTrue(methodGetTopLeft.isPresent());
        assertTrue(methodGetBottomRight.isPresent());
        assertTrue(methodSetTopLeft.isPresent());
        assertTrue(methodSetBottomRight.isPresent());
        assertTrue(methodContains.isPresent());


        assertTrue(Modifier.isPublic(methodGetTopLeft.get().getModifiers()));
        assertTrue(Modifier.isPublic(methodGetBottomRight.get().getModifiers()));

        assertTrue(Modifier.isPublic(methodSetTopLeft.get().getModifiers()));
        assertTrue(Modifier.isPublic(methodSetBottomRight.get().getModifiers()));

        assertTrue(Modifier.isPublic(methodContains.get().getModifiers()));


        assertTrue(methodGetTopLeft.get()
                .getReturnType()
                .isAssignableFrom(Point.class));
        assertTrue(methodGetBottomRight.get()
                .getReturnType()
                .isAssignableFrom(Point.class));

        assertTrue(methodSetTopLeft.get()
                .getReturnType()
                .isAssignableFrom(void.class));
        assertTrue(methodSetBottomRight.get()
                .getReturnType()
                .isAssignableFrom(void.class));

        assertTrue(methodContains.get()
                .getReturnType()
                .isAssignableFrom(boolean.class));


        assertEquals(1, methodSetTopLeft.get().getParameterCount());
        assertEquals(1, methodSetBottomRight.get().getParameterCount());

        assertTrue(Arrays.stream(methodSetTopLeft.get()
                .getParameterTypes())
                .anyMatch(p -> p.getSimpleName().equals(Point.class.getSimpleName())));

        assertTrue(Arrays.stream(methodSetBottomRight.get()
                .getParameterTypes())
                .anyMatch(p -> p.getSimpleName().equals(Point.class.getSimpleName())));

        assertTrue(Arrays.stream(methodContains.get()
                .getParameterTypes())
                .anyMatch(p -> p.getSimpleName().equals(Point.class.getSimpleName())));
    }
}
