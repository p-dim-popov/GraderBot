package eu.nvna.tests;

import eu.nvna.classes.Rectangle;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.Arrays;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;

public class TestPrivateMembers {
    private final Field[] fields = Rectangle.class.getDeclaredFields();

    @Test
    public void checkMembersCount() {
        assertEquals(4, fields.length);
    }

    @Test
    public void checkMembersModifiers() {
        assertTrue(Arrays.stream(fields)
                        .map(Field::getModifiers)
                        .allMatch(Modifier::isPrivate),
                "Some of the members were not private");
    }

    @Test
    public void checkMembersTypes() {
        assertTrue(Arrays.stream(fields)
                        .allMatch(f -> f.getType().equals(int.class)),
                "Some of the members were not type \"int\"");
    }

    @ParameterizedTest
    @ValueSource(strings = { "iX1", "iX2", "iY1", "iY2" })
    public void checkMembersNames(String name) {
        assertTrue(Arrays.stream(fields)
                .anyMatch(f -> f.getName().equals(name)));
    }
}
