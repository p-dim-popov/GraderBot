package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.Arrays;

import static org.junit.jupiter.api.Assertions.assertTrue;

public class TestPrivateFields {
    private final Field[] _fields = Rectangle.class.getDeclaredFields();

    @Test
    public void checkFieldsCount() {
        assertTrue(_fields.length >= 4,
                "Fields count was below required count!");
    }

    @Test
    public void checkFieldsModifiers() {
        assertTrue(Arrays.stream(_fields)
                        .map(Field::getModifiers)
                        .allMatch(Modifier::isPrivate),
                "Some of the fields were not private!");
    }

    @Test
    public void checkFieldsTypes() {
        assertTrue(Arrays.stream(_fields)
                        .allMatch(f -> f.getType().equals(int.class)),
                "Some of the members were not type \"int\"");
    }

    @ParameterizedTest
    @ValueSource(strings = { "iX1", "iX2", "iY1", "iY2" })
    public void checkFieldsNames(String name) {
        assertTrue(Arrays.stream(_fields)
                .anyMatch(f -> f.getName().equals(name)));
    }
}
