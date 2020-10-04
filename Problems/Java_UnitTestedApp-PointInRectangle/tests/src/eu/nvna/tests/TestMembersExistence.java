package eu.nvna.tests;

import eu.nvna.classes.Point;
import eu.nvna.classes.Rectangle;
import org.junit.Test;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.Arrays;
import java.util.Optional;
import java.util.function.Supplier;
import java.util.stream.Stream;

import static org.junit.Assert.*;

public class TestMembersExistence {
    @Test
    public void checkIfPointMembersExist() {
        Supplier<Stream<Field>> fieldsSupplier =
                () -> Arrays.stream(Point.class.getDeclaredFields());
        var fieldX = fieldsSupplier
                .get()
                .filter(f -> f.getName().equals("x"))
                .findFirst();
        var fieldY = fieldsSupplier
                .get()
                .filter(f -> f.getName().equals("y"))
                .findFirst();

        assertEquals(2, fieldsSupplier.get().count());
        assertTrue(fieldX.isPresent());
        assertTrue(fieldY.isPresent());
        assertTrue(Modifier.isPrivate(fieldX.get().getModifiers()));
        assertTrue(Modifier.isPrivate(fieldY.get().getModifiers()));
        assertTrue(fieldY.get().getType().isAssignableFrom(int.class));
        assertTrue(fieldY.get().getType().isAssignableFrom(int.class));
    }

    @Test
    public void checkIfRectangleMembersExist(){
        Supplier<Stream<Field>> fieldsSupplier =
                () -> Arrays.stream(Rectangle.class.getDeclaredFields());

        var fieldTopLeft = fieldsSupplier
                .get()
                .filter(f-> f.getName().equals("topLeft"))
                .findFirst();
        var fieldBottomRight = fieldsSupplier
                .get()
                .filter(f -> f.getName().equals("bottomRight"))
                .findFirst();

        assertEquals(2, fieldsSupplier.get().count());
        assertTrue(fieldTopLeft.isPresent());
        assertTrue(fieldBottomRight.isPresent());
        assertTrue(Modifier.isPrivate(fieldTopLeft.get().getModifiers()));
        assertTrue(Modifier.isPrivate(fieldBottomRight.get().getModifiers()));
        assertTrue(fieldTopLeft.get().getType().isAssignableFrom(Point.class));
        assertTrue(fieldBottomRight.get().getType().isAssignableFrom(Point.class));
    }
}
