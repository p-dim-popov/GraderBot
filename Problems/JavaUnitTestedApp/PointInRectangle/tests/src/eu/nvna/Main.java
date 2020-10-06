package eu.nvna;

import org.junit.runner.JUnitCore;
import org.junit.runner.Result;
import org.junit.runner.notification.Failure;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws ClassNotFoundException {
    	Scanner sc = new Scanner(System.in);
        Result result = JUnitCore.runClasses(Class.forName(sc.nextLine()));
        for (Failure failure : result.getFailures()) {
            System.out.println(failure.toString());
        }
        System.out.println("Result=" + result.wasSuccessful());
    }
}

