package eu.nvna.helpers;

import eu.nvna.classes.Color;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

public interface ReflectedColor {
    public static Color newInstance() throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        return Color.class
                .getConstructor()
                .newInstance();
    }

    public static Color newInstance(long rgb) throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        return Color.class
                .getConstructor(long.class)
                .newInstance(rgb);
    }

    public static long getFieldRValue(Color instance) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("RValue");
        field.setAccessible(true);
        return (short) field.get(instance);
    }

    public static short getFieldGValue(Color instance) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("GValue");
        field.setAccessible(true);
        return (short) field.get(instance);
    }

    public static short getFieldBValue(Color instance) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("BValue");
        field.setAccessible(true);
        return (short) field.get(instance);
    }

    static long getFieldRGBValue(Color color) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("RGB");
        field.setAccessible(true);
        return (long) field.get(color);
    }

    public static void setFieldRValue(Color instance, short value) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("RValue");
        field.setAccessible(true);
        field.set(instance, value);
    }

    public static void setFieldGValue(Color instance, short value) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("GValue");
        field.setAccessible(true);
        field.set(instance, value);
    }

    public static void setFieldBValue(Color instance, short value) throws NoSuchFieldException, IllegalAccessException {
        var field = Color.class.getDeclaredField("BValue");
        field.setAccessible(true);
        field.set(instance, value);
    }

    public static int compare(Color that, Object other) throws NoSuchMethodException, InvocationTargetException, IllegalAccessException {
        return (int) Color.class
                .getMethod("compareTo", Object.class)
                .invoke(that, other);
    }

    public static String toString(Color color) throws NoSuchMethodException, InvocationTargetException, IllegalAccessException {
        return (String) Color.class
                .getDeclaredMethod("toString")
                .invoke(color);
    }

    public static boolean areEqual(Color that, Object other) throws NoSuchMethodException, InvocationTargetException, IllegalAccessException {
        return (boolean) Color.class
                .getDeclaredMethod("equals", Object.class)
                .invoke(that, other);
    }

    public static Method getGetterRValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("getRValue");
    }

    public static Method getGetterGValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("getGValue");
    }

    public static Method getGetterBValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("getBValue");
    }

    public static Method getSetterSetRValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("setRValue", short.class);
    }

    public static Method getSetterSetGValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("setGValue", short.class);
    }

    public static Method getSetterSetBValue() throws NoSuchMethodException {
        return Color.class
                .getDeclaredMethod("setBValue", short.class);
    }
}
