package eu.nvna.classes;

import java.util.Objects;

public class Color implements Comparable {
    private short RValue;
    private short GValue;
    private short BValue;

    private long RGB;

    public Color() {
    }

    public Color(long c) {
        this.RGB = c;
        this.setSegregatedRGB(this.RGB);
    }

    public short getRValue() {
        return RValue;
    }

    public short getGValue() {
        return GValue;
    }

    public short getBValue() {
        return BValue;
    }

    public void setRValue(short rValue) {
        this.RGB += (long) (Math.pow(256, 2) * (-this.RValue + (this.RValue = rValue)));
    }

    public void setGValue(short gValue) {
        this.RGB += (long) (Math.pow(256, 1) * (-this.GValue + (this.GValue = gValue)));
    }

    public void setBValue(short bValue) {
        this.RGB += (long) (Math.pow(256, 0) * (-this.BValue + (this.BValue = bValue)));
    }

    @Override
    public String toString() {
        return "Color{" +
                "RValue=" + RValue +
                ", GValue=" + GValue +
                ", BValue=" + BValue +
                ", RGB=" + RGB +
                '}';
    }

    @Override
    public int compareTo(Object o) {
        if (o == null) return -2;
        if (this == o) return 0;
        if (!(o instanceof Color)) return -2;
        Color color = (Color) o;

        return Long.compare(this.RGB, color.RGB);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Color color = (Color) o;
        return RValue == color.RValue &&
                GValue == color.GValue &&
                BValue == color.BValue &&
                RGB == color.RGB;
    }

    @Override
    public int hashCode() {
        return Objects.hash(RValue, GValue, BValue, RGB);
    }

    private void setSegregatedRGB(long c) {
        this.RValue = (short) ((c / Math.pow(256, 2)) % 256);
        this.GValue = (short) ((c / Math.pow(256, 1)) % 256);
        this.BValue = (short) ((c / Math.pow(256, 0)) % 256);
    }

    public static void main(String[] args) {
        var x = 5;
        var s = 20;
        s = s - x + (x = 10);
        System.out.println(s);
        System.out.println(x);
    }
}
