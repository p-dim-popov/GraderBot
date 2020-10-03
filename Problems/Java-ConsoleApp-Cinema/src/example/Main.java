package example;

import java.util.Scanner;

class Project01 {
    public static void main(String[] args) {
        var scanner = new Scanner(System.in);
        String projection = scanner.nextLine();
        int rows = Integer.parseInt(scanner.nextLine());
        int cols = Integer.parseInt(scanner.nextLine());

        double income = 0.0;

        if ("Premiere".equals(projection)) {
            income = rows * cols * 12;
        } else if ("Normal".equals(projection)) {
            income = rows * cols * 7.50;
        } else if ("Discount".equals(projection)) {
            income = rows * cols * 5;
        }

        System.out.printf("%.2f", income);
    }
}