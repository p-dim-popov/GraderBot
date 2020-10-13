package eu.nvna;

import org.junit.platform.launcher.core.LauncherDiscoveryRequestBuilder;
import org.junit.platform.launcher.core.LauncherFactory;
import org.junit.platform.launcher.listeners.SummaryGeneratingListener;
import org.junit.platform.launcher.listeners.TestExecutionSummary;

import java.util.Scanner;

import static org.junit.platform.engine.discovery.DiscoverySelectors.selectClass;

public class Main {
    public static void main(String[] args) throws ClassNotFoundException {
        Scanner sc = new Scanner(System.in);

        final var request =
                LauncherDiscoveryRequestBuilder
                        .request()
                        .selectors(selectClass(Class.forName(sc.nextLine())))
                        .build();

        final var launcher = LauncherFactory.create();
        final var listener = new SummaryGeneratingListener();

        launcher.registerTestExecutionListeners(listener);
        launcher.execute(request);

        TestExecutionSummary summary = listener.getSummary();

        for (var failure : summary.getFailures()) {
            System.out.println(failure.getTestIdentifier().getDisplayName() + " failed!");
            System.out.println(failure.getException().toString());
        }

        System.out.println("Tests=" + summary.getTestsFoundCount() + ","
                + "Passed=" + summary.getTestsSucceededCount());
    }
}

