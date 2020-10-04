package eu.nvna;

import org.junit.runner.JUnitCore;
import org.junit.runner.Result;
import org.junit.runner.notification.Failure;

public class Main {
    public static void main(String[] args) throws ClassNotFoundException {
        Result result = JUnitCore.runClasses(Class.forName("eu.nvna.tests.TestMembersExistence"));
        for (Failure failure : result.getFailures()) {
            System.out.println(failure.toString());
        }
        System.out.println("Result==" + result.wasSuccessful());
    }
}

