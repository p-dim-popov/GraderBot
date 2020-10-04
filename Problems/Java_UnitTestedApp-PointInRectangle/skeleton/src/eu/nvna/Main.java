package eu.nvna;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Arrays;
import eu.nvna.figures.*;

public class Main {
    /*
    * Input
    * On the first line read the coordinates of the bottom left and top right corner of the Rectangle
    * in the format: “<bottomLeftX> <bottomLeftY> <topRightX> <topRightY>”.
    * On the second line, read an integer N and on the next N lines, read the coordinates of points.
    * Output
    * For each point, print out the result of the Contains() method.
    *
    * Examples:

    * input:
        0 0 3 3
        5
        0 0
        0 1
        4 4
        5 3
        1 2
    * output:
        true
        true
        false
        false
        true
    * */

    public static void main(String[] args) throws IOException {
        var reader = new BufferedReader(new InputStreamReader(System.in));

        int[] rectangleCoords = getCoords(reader);

        Point topLeft = new Point(rectangleCoords[0], rectangleCoords[1]);
        Point bottomRight = new Point(rectangleCoords[2], rectangleCoords[3]);

        Rectangle rectangle = new Rectangle(topLeft, bottomRight);

        int count = Integer.parseInt(reader.readLine());

        for (int i = 0; i < count; i++) {
            int[] pointCoords = getCoords(reader);

            Point point = new Point(pointCoords[0], pointCoords[1]);

            System.out.println(rectangle.contains(point));
        }
    }

    private static int[] getCoords(BufferedReader reader) throws IOException {
        return Arrays.stream(reader.readLine().split(" "))
                .mapToInt(Integer::parseInt)
                .toArray();
    }
}