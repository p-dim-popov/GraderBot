package eu.nvna.tests;

import eu.nvna.classes.Point;
import eu.nvna.classes.Rectangle;
import org.junit.Test;

import static org.junit.Assert.*;

public class TestFunctionality {

    @Test
    public void checkIfPointMethodsWorkAsExpected(){
        var point = new Point(4, 5);

        assertEquals(4, point.getX());
        assertEquals(5, point.getY());

        point.setX(1);
        point.setY(2);

        assertEquals(1, point.getX());
        assertEquals(2, point.getY());
    }

    @Test
    public void checkIfRectangleMethodsWorkAsExpected(){
        var rectangle = new Rectangle(
                new Point(1,2),
                new Point(4, 5));

        assertEquals(rectangle.getTopLeft().getX(), 1);
        assertEquals(rectangle.getTopLeft().getY(), 2);
        assertEquals(rectangle.getBottomRight().getX(), 4);
        assertEquals(rectangle.getBottomRight().getY(), 5);

        rectangle.setTopLeft(new Point(0, 0));
        rectangle.setBottomRight(new Point(6, 6));

        assertEquals(rectangle.getTopLeft().getX(), 0);
        assertEquals(rectangle.getTopLeft().getY(), 0);
        assertEquals(rectangle.getBottomRight().getX(), 6);
        assertEquals(rectangle.getBottomRight().getY(), 6);

        assertTrue(rectangle.contains(new Point(3, 3)));
    }
}
