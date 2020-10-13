package eu.nvna.tests;

import eu.nvna.helpers.ReflectedColor;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;

public class TestEquals {
    @Test
    public void equals_ShouldBehaveNormally() throws InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException {
        var modelColor = (long) (Math.pow(256, 2) * 234 + Math.pow(256, 1) * 123 + Math.pow(256, 0) * 12);
        var notEqualColor = (long) (Math.pow(256, 2) * 34 + Math.pow(256, 1) * 123 + Math.pow(256, 0) * 12);

        var model = ReflectedColor.newInstance(modelColor);
        var notEqual = ReflectedColor.newInstance(notEqualColor);
        var equalToModel = ReflectedColor.newInstance(modelColor);

        Assertions.assertFalse(ReflectedColor.areEqual(model, notEqual));
        Assertions.assertTrue(ReflectedColor.areEqual(model, equalToModel));
        Assertions.assertFalse(ReflectedColor.areEqual(model, null));
        Assertions.assertFalse(ReflectedColor.areEqual(model, "I'm color also!"));
    }
}
